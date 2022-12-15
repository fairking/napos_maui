<template>
    <q-page padding>
        <h4>About</h4>
        <p>Lorem ipsum dolor sit amet consectetur adipisicing elit. Fugit nihil praesentium molestias a adipisci, dolore vitae odit, quidem consequatur optio voluptates asperiores pariatur eos numquam rerum delectus commodi perferendis voluptate?</p>
    </q-page>
</template>

<script lang="ts">
    import { defineComponent } from 'vue';

    export default defineComponent({
        name: 'AboutPage',
        data() {
            return {
                themeOptions: [
                    {
                        label: 'System',
                        value: null
                    },
                    {
                        label: 'Light',
                        value: false
                    },
                    {
                        label: 'Dark',
                        value: true
                    }
                ],
                settings: {
                    theme: null
                },
            };
        },
        beforeMount() {
            this.setTheme();
        },
        methods: {
            setTheme() {
                this.$q.dark.set(this.settings.theme ?? 'auto');
            },
            onThemeChange() {
                this.setTheme();
                this.onChange();
            },
            onChange() {
                // Save settings
            },
        }
    });
</script>
