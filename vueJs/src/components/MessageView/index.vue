<template>
  <div>
    <virtual-list
      class="list-infinite scroll-touch"
      :data-key="'id'"
      :data-sources="items"
      :data-component="itemComponent"

      :estimate-size="70"
      :item-class="'list-item-infinite'"
      :footer-class="'loader-wrapper'"
    />
    <Pagination
      v-show="totalCount>0"
      :total="totalCount"
      :page.sync="page"
      :limit.sync="limit"
      @pagination="onPageChanged"
    />
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator'
import Item, { Message } from './item.vue'
import Pagination from '@/components/Pagination/index.vue'

@Component({
  name: 'MessageView',
  components: {
    Item,
    Pagination
  }
})
export default class extends Vue {
  @Prop({ default: '' }) private id!: string
  @Prop({ default: 0 }) private totalCount!: number
  @Prop({ default: new Array<Message>() }) private items!: Message[]
  private page = 1
  private limit = 20
  private itemComponent!: Item

  private onPageChanged() {
    this.$emit('onPageChanged', { id: this.id, page: this.page, pageSize: this.limit })
  }
}
</script>

<style lang="scss">
.result {
  margin-bottom: 1em;
}
.list-infinite {
  width: 100%;
  height: 500px;
  border: 2px solid;
  border-radius: 3px;
  overflow-y: auto;
  border-color: dimgray;
  position: relative;
  .list-item-infinite {
    display: flex;
    align-items: center;
    padding: 1em;
    border-bottom: 1px solid;
    border-color: lightgray;
  }
  .loader-wrapper {
    padding: 1em;
  }
  .loader {
    font-size: 10px;
    margin: 0px auto;
    text-indent: -9999em;
    width: 30px;
    height: 30px;
    border-radius: 50%;
    background: #ffffff;
    background: linear-gradient(to right, #9b4dca 10%, rgba(255, 255, 255, 0) 42%);
    position: relative;
    animation: load3 1.4s infinite linear;
    transform: translateZ(0);
  }
  .loader:before {
    width: 50%;
    height: 50%;
    background: #9b4dca;
    border-radius: 100% 0 0 0;
    position: absolute;
    top: 0;
    left: 0;
    content: '';
  }
  .loader:after {
    background: #ffffff;
    width: 75%;
    height: 75%;
    border-radius: 50%;
    content: '';
    margin: auto;
    position: absolute;
    top: 0;
    left: 0;
    bottom: 0;
    right: 0;
  }
  @-webkit-keyframes load3 {
    0% {
      transform: rotate(0deg);
    }
    100% {
      transform: rotate(360deg);
    }
  }
  @keyframes load3 {
    0% {
      transform: rotate(0deg);
    }
    100% {
      transform: rotate(360deg);
    }
  }
}
</style>
