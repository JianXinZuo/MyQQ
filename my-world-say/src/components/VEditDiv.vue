<template>
    <div contenteditable="canEdit" 
    v-text="innerText"
    @focus="isLocked = true"
    @blur="isLocked = false"
    @input="handleInput"
    ></div>
</template>
<script>
    export default {
        name: 'VEditDiv',
        props: {
            value: {
                type: String,
                default: ''
            },
            canEdit: {
                type: Boolean,
                default: true
            }
        },
        data(){
            return {
                innerText: '',
                isLocked: false
            }
        },
        watch: {
            'value'(){
                if (!this.isLocked && !this.innerText) {
                    this.innerText = this.value;
                }
            }
        },
        methods: {
            handleInput($event){
                this.$emit('input', $event.target.innerText);
            }
        }
    }
</script>