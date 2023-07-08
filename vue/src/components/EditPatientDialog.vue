<template>
    <v-dialog v-model="dialogOpen">
        <v-card>
            <v-card-title>
                <h2>Edit Patient</h2>
            </v-card-title>
            <v-row>
                <v-col cols="6">
                    <v-card-text>
                        <v-form>
                            <v-text-field v-model="patientData.personName"
                                          :rules="rules"
                                          label="First name"></v-text-field>
                            <v-text-field v-model="patientData.personSurname"
                                          :rules="rules"
                                          label="Last name"></v-text-field>
                            <v-text-field type="text"
                                          v-model="formattedBirthdate"
                                          label="Birthdate"
                                          :rules="birthdateRules"></v-text-field>
                        </v-form>
                    </v-card-text>
                </v-col>
                <v-col cols="6">
                    <v-checkbox v-for="diagnose in diagnoses"
                                :key="diagnose.id"
                                v-model="selectedDiagnoseIds"
                                :label="diagnose.title"
                                :value="diagnose.id"></v-checkbox>
                </v-col>
            </v-row>

            <v-card-actions>
                <v-btn @click="dialogOpen = false">Close</v-btn>
                <v-btn color="primary" @click="saveChanges">Save</v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script>
    import axios from "axios";
    import { th } from "vuetify/locale";

    export default {
        props: {
            patientData: {
                type: Object,
                required: true,
            },
        },
        data() {
            return {
                dialogOpen: true,
                diagnoses: [],
                selectedDiagnoseIds: [],
                formattedBirthdate: new Date(this.patientData.personBirthdate).toLocaleDateString(),
                rules: [
                    value => {
                        if (value) return true;
                        return 'You must enter a name.';
                    },
                ],
                birthdateRules: [
                    value => /^\d{2}\.\d{2}\.\d{4}$/.test(value) || 'Invalid date format (dd.mm.yyyy)',
                ]
            };
        },
        mounted() {
            axios
                .get("https://localhost:7109/api/Diagnose")
                .then((response) => {
                    this.diagnoses = response.data;
                    this.selectedDiagnoseIds = this.patientData.diagnoses.map((diagnose) => diagnose.id);
                })
                .catch((error) => {
                    console.error(error);
                });
        },
        methods: {
            saveChanges() {
                const updatedPerson = {
                    id: this.patientData.personId,
                    name: this.patientData.personName,
                    surname: this.patientData.personSurname,
                    birthdate: this.patientData.personBirthdate,
                };

                axios
                    .put(`https://localhost:7109/api/Person/${updatedPerson.id}`, {
                        name: updatedPerson.name,
                        surname: updatedPerson.surname,
                        birthdate: updatedPerson.birthdate,
                    })
                    .then((response) => {
                        const updatedPatient = {
                            id: this.patientData.id,
                            personId: updatedPerson.id,
                            diagnoseIds: this.selectedDiagnoseIds,
                        };
                        axios
                            .put(`https://localhost:7109/api/Patient/${updatedPatient.id}`, updatedPatient)
                            .then((response) => {
                                this.$emit("close");
                            })
                            .catch((error) => {
                                console.error("Request failed:", error.response.data);
                            });
                    })
                    .catch((error) => {
                        console.error("Request failed:", error.response.data);
                    });
                this.dialogOpen = false
            },
            formatDate(date) {
                const formattedDate = new Date(date).toLocaleDateString();
                return formattedDate;
            },
        },
    };
</script>
