<template>
  <v-container fluid>
    <v-app-bar color="black">
      <v-btn>
        <router-link to="/">List of patients</router-link>
      </v-btn>
      <v-btn >
        <router-link to="/createpatient">Create a new patient</router-link>
      </v-btn>
    </v-app-bar>
  </v-container>
</template>

<script>
export default {

}
</script>

<style lang="scss" scoped>

</style>