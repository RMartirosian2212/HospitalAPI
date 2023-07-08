<template>
  <v-main>
    <v-container fluid>
      <v-table>
        <thead>
        <tr>
          <th class="text-left">Id</th>
          <th class="text-left">Name</th>
          <th class="text-left">Surname</th>
          <th class="text-left">Birthday</th>
          <th class="text-left">Diagnoses</th>
          <th class="text-left">Actions</th>
        </tr>
        </thead>
        <tbody>
        <tr v-for="item in patients" :key="item.id">
          <td>{{ item.id }}</td>
          <td>{{ item.person.name }}</td>
          <td>{{ item.person.surname }}</td>
          <td>{{ formatDate(item.person.birthdate) }}</td>
          <td>
              <span v-for="(diagnose, index) in item.diagnoses" :key="diagnose.id">
                {{ diagnose.title }}
                <span v-if="index !== item.diagnoses.length - 1">, </span>
              </span>
          </td>
          <td>
            <v-btn color="primary" style="margin-right: 10px" @click="openEditDialog(item)">Edit</v-btn>
            <v-btn color="error" small @click="deletePatient(item.id)">Delete</v-btn>
          </td>
        </tr>
        </tbody>
      </v-table>
    </v-container>
    <edit-patient-dialog v-if="dialogOpen" :patientData="selectedPatient" @close="closeEditDialog"></edit-patient-dialog>
  </v-main>
</template>

<script>
import axios from 'axios';
import EditPatientDialog from '@/components/EditPatientDialog.vue';

export default {
  components: {
    EditPatientDialog,
  },
  data() {
    return {
      patients: [],
      selectedPatient: null,
      dialogOpen: false,
    };
  },
  mounted() {
    axios
        .get('https://localhost:7109/api/Patient')
        .then((response) => {
          this.patients = response.data;
        })
        .catch((error) => {
          console.error(error);
        });
  },
  methods: {
    deletePatient(patientId) {
      axios
          .delete(`https://localhost:7109/api/Patient/${patientId}`)
          .then(() => {
            this.patients = this.patients.filter(patient => patient.id !== patientId);
          })
          .catch((error) => {
            console.error('Request failed:', error.response.data);
          });
    },
    openEditDialog(patient) {
      axios.get(`https://localhost:7109/api/Person/${patient.personId}`)
          .then(response => {
            const person = response.data;
            this.selectedPatient = {
              id: patient.id,
              personId: patient.personId,
              personName: person.name,
              personSurname: person.surname,
              personBirthdate: person.birthdate,
              diagnoses: patient.diagnoses
            };
            this.dialogOpen = true;
          })
          .catch(error => {
            console.error('Failed to fetch person data:', error);
          });
      this.dialogOpen = false;
    },
    closeEditDialog() {
      this.dialogOpen = false;
      console.log(this.dialogOpen)
      this.selectedPatient = null;
    },
    formatDate(date) {
      const formattedDate = new Date(date).toLocaleDateString();
      return formattedDate;
    },
  },
};
</script>

<style lang="scss" scoped>
</style>
