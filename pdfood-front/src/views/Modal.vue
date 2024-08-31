<script setup lang="ts">
import { defineModel, defineEmits } from 'vue';
import Button from 'primevue/button';
import Dialog from 'primevue/dialog';
import InputText from 'primevue/inputtext';
import FileUpload from 'primevue/fileupload';
import InputNumber from 'primevue/inputnumber';
import { Product, Image } from '../types/Product';
import axios from "axios"

const api = axios.create({
    baseURL: import.meta.env.VITE_API_BASE_URL
})

const visible = defineModel<boolean>('visible');

const prod = defineModel<Product>({
    required: false,
    default: {
        image: "",
        name: "",
        price: 0,
        barCode: ""
    }
});

const emit = defineEmits<{
    (e: 'submit-product-form'): void
    (e: 'on-cancel-click'): void
    (e: 'update-image', file: any): void
}>()

const uploadImage = async (data: any) => {
    const selectedFile = data.files[0],
        formData = new FormData();
        
    console.log(selectedFile);

    formData.append('formFile', selectedFile)
    
    const response = await api.post('imageUpload', formData, {
        headers: {
            "Content-Type": "multipart/form-data",
        }
    })
console.log(response.data)
    prod.value.image = response.data;
}

</script>

<template>
    <Dialog v-model:visible="visible" modal header="Produto: " :style="{ width: '600px' }">
        <div class="flex flex-col gap-4">
            <div class="flex flex-col gap-2">
                <label for="name">Nome</label>
                <InputText id="name" v-model="prod.name" />
            </div>
            <div class="flex flex-col gap-2">
                <label for="price">Preço</label>
                <InputNumber id="price" v-model="prod.price" />
            </div>
            <div class="flex flex-col gap-2">
                <label for="barCode">Código de barras</label>
                <InputText id="barCode" v-model="prod.barCode"  />
            </div>
            <div class="flex flex-col gap-2 ">
                <h5>Imagem</h5>
                <img v-if="prod.image" :src="prod.image.url" class="w-[300px] h-[300px] object-cover" />
                <FileUpload choose-label="Selecionar imagem" mode="basic" name="image" accept="image/*" :max-file-size="1000000" :custom-upload="true" @uploader="uploadImage" :auto="true" />
            </div>
        </div>
        <div class="flex w-full gap-2 justify-end my-4">
            <Button label="Salvar" @click="() => emit('submit-product-form')" />
            <Button label="Cancelar" @click="() => emit('on-cancel-click')" severity="danger" class="text-white" />
        </div>
    </Dialog>
</template>