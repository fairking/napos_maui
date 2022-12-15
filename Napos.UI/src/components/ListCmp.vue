<template>
    <div class="q-pa-md" style="max-width: 480px">
        <q-list bordered separator>

            <q-item clickable v-ripple v-for="item in items" :key="item.id">
                <slot name="itemTemplate" :item="item"></slot>
            </q-item>

        </q-list>
    </div>
</template>

<script lang="ts">
    import { defineComponent } from 'vue';

    export default defineComponent({
        name: 'ListCmp',

        props: {
            items: [],
        },

        data() {
            return {
            };
        },

        methods: {
        }
    });
</script>
