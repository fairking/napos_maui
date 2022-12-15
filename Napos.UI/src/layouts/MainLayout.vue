<template>
    <q-layout view="hhh LpR fFf">
        <q-header reveal bordered>
            <q-toolbar>
                <q-btn flat
                       dense
                       round
                       icon="menu"
                       aria-label="Menu"
                       @click="toggleLeftDrawer" />

                <q-toolbar-title>
                    Napos App
                </q-toolbar-title>

                <div><q-btn v-if="isBeta" round color="negative" size="sm" icon="warning_amber" @click="betaWarning()" /> v.{{ $version }}</div>
            </q-toolbar>
        </q-header>

        <q-drawer show-if-above v-model="leftDrawerOpen" side="left" bordered>
            <MenuCmp />
        </q-drawer>

        <q-page-container>
            <router-view />
        </q-page-container>
    </q-layout>
</template>

<script lang="ts">
    import { defineComponent } from 'vue';
    import MenuCmp from 'src/components/MenuCmp.vue';

    export default defineComponent({
        name: 'MainLayout',

        components: {
            MenuCmp
        },

        computed: {
            isBeta() {
                return this.$version.includes('beta');
            },
        },

        data() {
            return {
                leftDrawerOpen: false,
            }
        },

        beforeMount() {
            //debugger;
            //console.log(this);
        },

        methods: {
            toggleLeftDrawer() {
                this.leftDrawerOpen = !this.leftDrawerOpen
            },
            betaWarning() {
                alert('This is a beta version. Your data can disappear or become inconsistent. Please use it for testing purposes only.');
            },
        }
    });
</script>
