<script setup lang="ts">
import Button from 'primevue/button';
import InputText from 'primevue/inputtext';
import { ref, onMounted, computed } from 'vue';
import Modal from '../views/Modal.vue';
import Table from '../views/Table.vue';
import { Product, Image } from '../types/Product.ts'
import { Pageable } from '../types/Pageable.ts'
import axios from "axios"

const api = axios.create({
    baseURL: import.meta.env.VITE_API_BASE_URL
})

const filterBarCode = ref<string>('')
const filterProductName = ref<string>('')
const modalVisible = ref<boolean>(false);
const limit = ref<number>(10);
const reponseData = ref<Pageable>([]);
const reponseContent = ref<Product[]>([]);

const emptyProduct = ref<Product>({
    barCode: "",
    id: 0,
    image: "",
    price: 0,
    name: ""
});

const selectedProduct = emptyProduct;

const onSubmitForm = async () => {
  try {
    if (!selectedProduct.value.id) {
      await api.post('product', selectedProduct.value);
    } else {
      await api.put(`product/${selectedProduct.value.id}`, selectedProduct.value);
    }

    selectedProduct.value = emptyProduct;

    findAllProducts();

    modalVisible.value = false;
  } catch (err) {
    console.error(err);
  }
}

const onRemoveProduct = async (id: Number) => {
  try {
    await api.delete(`product/${id}`);

    findAllProducts();
  } catch (err) {
    console.error(err);
  }
}

const onCancelClick = async () => {
    selectedProduct.value = emptyProduct;
    modalVisible.value = false;
}

const findAllProducts = async (offset?:number = 0) => {
  var url = `product?offset=` + (offset ?? 0) + `&limit=` + limit.value;

  if (filterBarCode.value) { 
    url += `&barCode=${filterBarCode.value}`;
  }

  if (filterProductName.value) {
      url += `&name=${filterProductName.value}`
  }
  
  const response = await api.get<Pageable>(url);
  
  if (response.status == 200) {
    console.log(response.data)
    console.log(response.data.content.length)
    reponseData.value = response.data;
    reponseContent.value = response.data.content;
  }
}

const editProduct = (editP: Product) => {
  
  selectedProduct.value = {
    barCode: editP.barCode,
    id: editP.id,
    image: editP.image,
    price: editP.price,
    name: editP.name
  }

  modalVisible.value = true;
}

const resetProductForm = () => {
  selectedProduct.value = {
    barCode: "",
    id: 0,
    imageUrl: "",
    price: 0,
    name: "",
  }
}

const toNextPage = () => {
  var offset = reponseData.value.offset + limit.value;
  findAllProducts(offset);
}

const toPreviousPage = () => {
  var offset = reponseData.value.offset - limit.value;

  if (offset < 0) {
    offset = 0;
  }

  findAllProducts(offset);
}

onMounted(() => {
  findAllProducts();
});

</script>

<template>
    <main class="bg-slate-100 w-full min-h-[h-screen] flex items-center justify-center">
    <div class="w-3/4 flex flex-col items-center justify-center my-25">
      <h1 class="text-slate-700 text-4xl font-bold ">Produtos</h1>
      <div class="w-full flex justify-between items-center">
        <div class="flex gap-2 my-4 text-white">
          <div class="flex gap-2">
              <InputText placeholder="Código de barras" type="text" v-model="filterBarCode" />
              <InputText placeholder="Nome do produto" type="text" v-model="filterProductName" />
          </div>
          <Button icon="pi pi-search" aria-label="Pesquisar"  type="button" @click="() => findAllProducts(reponseData?.value?.offset)" />
          <Button icon="pi pi-refresh" aria-label="Atualizar"  type="button" @click="() => findAllProducts(reponseData?.value?.offset)" />
        </div>
        <Button label="Adicionar Produto" icon="pi pi-plus" @click="modalVisible = true" />
      </div>
      <Table :data="reponseData?.content" @edit-product="editProduct" @remove-product="onRemoveProduct" />
      <div class="w-full flex justify-between items-end">
        <p class="text-sm text-slate-600">Mostrando {{ reponseData?.content?.length ?? 0 }} registros</p>
        <div class="flex justify-end gap-1 my-2">
          <Button :disabled="(reponseData?.offset ?? 0) == 0" icon="pi pi-arrow-left" label="Anterior" @click="() => toPreviousPage()" />
          <Button :disabled="!reponseData?.hasNext" icon="pi pi-arrow-right" label="Próxima" icon-pos="right" @click="() => toNextPage()" />
        </div>
      </div>
    </div>
    <Modal v-model:visible="modalVisible" v-model="selectedProduct" @submit-product-form="onSubmitForm" @on-cancel-click="onCancelClick" />
  </main>
</template>