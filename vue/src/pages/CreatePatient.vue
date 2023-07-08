<template>
    <v-main>
        <v-sheet width="750" class="mx-auto">
            <v-row>
                <v-col cols="6">
                    <v-form @submit.prevent="submitForm">
                        <v-text-field v-model="firstName"
                                      :rules="rules"
                                      type="text"
                                      label="First name"></v-text-field>
                        <v-text-field v-model="lastName"
                                      :rules="rules"
                                      type="text"
                                      label="Last name"></v-text-field>
                        <v-text-field type="text"
                                      v-model="birthdateInput"
                                      label="Birthdate"
                                      :rules="birthdateRules"></v-text-field>
                        <v-btn type="submit" block class="mt-2">Submit</v-btn>
                    </v-form>
                </v-col>
                <v-col cols="6">
                    <v-checkbox v-for="diagnose in diagnoses"
                                :key="diagnose.id"
                                v-model="selected_diagnoses"
                                :label="diagnose.title"
                                :value="diagnose.id"></v-checkbox>
                </v-col>
            </v-row>
        </v-sheet>
    </v-main>
</template>

<script>
    import axios from 'axios';

    export default {
        data() {
            return {
                firstName: '',
                lastName: '',
                birthdateInput: '',
                diagnoses: null,
                selected_diagnoses: [],
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
            axios.get('https://localhost:7109/api/Diagnose')
                .then(response => this.diagnoses = response.data)
                .catch(error => {
                    console.error(error);
                });
        },
        methods: {
            async submitForm() {
                const formattedBirthdate = this.formatDateISO(this.birthdateInput);
                const PersonData = {
                    Name: this.firstName.toString(),
                    Surname: this.lastName.toString(),
                    Birthdate: formattedBirthdate.toString()
                };

                await fetch('https://localhost:7109/api/Person', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(PersonData)
                })
                    .then(response => {
                        if (!response.ok) {
                            throw new Error('Request failed with status ' + response.status);
                        }
                        return response.json();
                    })
                    .then(data => {
                        const personId = data.id;
                        console.log(personId);
                        const patientData = {
                            PersonId: personId,
                            DiagnoseIds: this.selected_diagnoses,
                        };
                        console.log(patientData.PersonId);
                        console.log(patientData.DiagnoseIds)

                        return fetch('https://localhost:7109/api/Patient', {
                            method: 'POST',
                            headers: {
                                'Content-Type': 'application/json'
                            },
                            body: JSON.stringify(patientData)
                        });
                    })
                    .then(response => {
                        console.log(response.data);
                        this.$router.push('/');
                    })
                    .catch(error => {
                        console.error('Request failed:', error.message);
                    });
            },
            formatDateISO(date) {
                const formattedDate = new Date(date).toISOString();
                return formattedDate;
            }
        }
    };
</script>
