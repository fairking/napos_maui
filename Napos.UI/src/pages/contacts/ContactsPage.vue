<template>
    <q-page padding>
        <h4 class="q-ma-sm">Contacts</h4>

        <div class="q-pa-md">

            <div class="row">
                <div class="col">
                    <q-input v-model="search"
                             debounce="500"
                             filled
                             placeholder="Search"
                             clearable>
                        <!--<template v-slot:append>
                            <q-icon name="search" />
                        </template>-->
                    </q-input>
                </div>
                <div class="col col-auto">
                    <q-btn color="primary" label="Create New" @click="createContact" class="q-pa-md" />
                </div>
            </div>

        </div>
        

        <ListCmp :items="filteredList">
            <template #itemTemplate="slotProps">
                <q-item-section>{{ slotProps.item.name }}</q-item-section>
            </template>
        </ListCmp>
    </q-page>
</template>

<script lang="ts">
    import { defineComponent } from 'vue';
    import ListCmp from 'src/components/ListCmp.vue';
    import { ContactService, ContactItem, CreateContactForm } from 'src/services/domain';

    export default defineComponent({
        name: 'ContactsPage',

        components: {
            ListCmp
        },

        data() {
            return {
                search: "",
                items: Array<ContactItem>(),
            };
        },

        computed: {
            filteredList(): ContactItem[] {
                if (this.search.length > 0)
                    return this.items.filter(x => x.name?.includes(this.search));
                else
                    return this.items;
            },
        },

        beforeMount() {
            this.loadItems();
        },

        methods: {
            async loadItems() {
                this.items = await ContactService.getList();
            },
            async createContact() {
                let name = 'Name ' + Math.random().toString(36).slice(1, 7);
                await ContactService.create({ body: new CreateContactForm({ name: name }) });
                await this.loadItems();
            },
        }
    });
</script>
