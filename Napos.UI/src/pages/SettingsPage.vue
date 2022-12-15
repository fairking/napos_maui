<template>
    <q-page padding>
        <h4>Theme</h4>
        <q-option-group v-model="settings.theme"
                        :options="themeOptions"
                        color="primary"
                        inline
                        @update:model-value="onThemeChange" />
        <h4>Stores</h4>
        <p>List of stores</p>
    </q-page>
</template>

<script lang="ts">
    import { defineComponent } from 'vue';
    import { SettingService, SettingsForm } from 'src/services/domain';

    export default defineComponent({
        name: 'SettingsPage',
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
                settings: {} as SettingsForm,
            };
        },
        async beforeMount() {
            await this.getSettings();
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
                this.setSettings();
            },
            async getSettings() {
                this.settings = await SettingService.get();
           },
            setSettings() {
                SettingService.save({ body: this.settings });
            }
        }
    });
</script>
