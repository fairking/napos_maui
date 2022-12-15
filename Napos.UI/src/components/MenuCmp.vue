<template>
    <q-list>
        <q-item-label header>
            Menu
        </q-item-label>

        <q-item :to="menuItem.to" exact clickable v-for="menuItem in menuItems" :key="menuItem.name">
            <q-item-section v-if="menuItem.icon" avatar>
                <q-icon :name="menuItem.icon" />
            </q-item-section>
            <q-item-section>
                {{ menuItem.title }}
            </q-item-section>
        </q-item>
    </q-list>
</template>

<script lang="ts">
    import { defineComponent } from 'vue';

    export default defineComponent({
        name: 'MenuCmp',
        data() {
            return {
                // Please use the following icons: https://fonts.google.com/icons?selected=Material+Icons
                menuItems: [
                    {
                        name: 'dashboard',
                        icon: 'home',
                        title: 'Dashboard',
                        to: '/',
                    },
                    {
                        name: 'contacts',
                        icon: 'alternate_email',
                        title: 'Contacts',
                        to: '/contacts',
                    },
                    {
                        name: 'settings',
                        icon: 'settings',
                        title: 'Settings',
                        to: '/settings',
                    },
                    {
                        name: 'about',
                        icon: 'help',
                        title: 'About & Help',
                        to: '/about',
                    },
                ]
            };
        },
    });
</script>
