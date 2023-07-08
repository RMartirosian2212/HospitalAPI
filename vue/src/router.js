import { createRouter, createWebHistory } from 'vue-router';
import MainPage from "@/pages/MainPage.vue";
import CreatePatient from "@/pages/CreatePatient.vue";

const routes = [
    {
        path: '/',
        component: MainPage,
        meta: {
            noCache: true // Добавьте это свойство в объект meta
        }
    },
    {
        path: '/createpatient',
        component: CreatePatient,
        meta: {
            noCache: true // Добавьте это свойство в объект meta
        }
    }
];

const router = createRouter({
    history: createWebHistory(),
    routes
});

export default router;
