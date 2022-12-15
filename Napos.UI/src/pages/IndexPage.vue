<template>
    <q-page class="row items-center justify-evenly">
        <q-input v-model="item.name" label="Name: " />
        <q-btn @click="createStore" label="Create" />
        <q-btn @click="loadStores" label="Load" />
        <ul>
            <li v-for="itm in list" :key="itm.id">{{ itm.name }}</li>
        </ul>
    </q-page>
</template>

<script lang="ts">
    import { defineComponent } from 'vue';
    import { CreateStoreForm, StoreItem, StoreService } from 'src/services/domain';

    export default defineComponent({
        name: 'IndexPage',
        data() {
            return {
                item: new CreateStoreForm(),
                list: Array<StoreItem>(),
            };
        },
        methods: {
            createStore() {
                StoreService.create({ body: this.item }).then(() => {
                    this.item = new CreateStoreForm();
                });
            },
            loadStores() {
                StoreService.getList().then(result => {
                    this.list.length = 0;
                    this.list.push(...result);
                });
            },
            loadEnvs() {
                console.log(process.env);
            }
        }
    });
</script>
