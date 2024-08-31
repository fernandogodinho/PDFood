<script setup lang="ts">
import Button from 'primevue/button';
import Column from 'primevue/column';
import DataTable from 'primevue/datatable';
import { Product } from '../types/Product';

const props = defineProps<{
    data: Product[]
}>()

const emit = defineEmits<{
  (e: 'edit-product', prod: Product): void
  (e: 'remove-product', id: Number): void
}>();
</script>

<style scoped>
.product-options {
  display: flex;
  gap: 10px;
}
</style>

<template>
    <DataTable :value="props.data" style="min-width: 100%">
        <Column field="id" header="Código" style="width: 8%"></Column>
        <Column field="barCode" header="Código de barras" style="width: 25%"></Column>
        <Column field="name" header="Nome" style="width: 45%"></Column>
        <Column field="price" header="Preço" style="width: 20%" :sortable="true"></Column>
        <Column field="id">
          <template #body="slotProps">
            <div class="product-options">
              <Button icon="pi pi-pencil" @click="() => emit('edit-product', slotProps.data)" />
              <Button icon="pi pi-trash" severity="danger"  @click="() => emit('remove-product', slotProps.data.id)" />
            </div>
          </template>
        </Column>
      </DataTable>
</template>