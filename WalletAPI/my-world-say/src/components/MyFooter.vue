<template>
    <v-bottom-nav
      :active.sync="bottomNav"
      color="blue"
      :value="true"
      absolute
      shift
    >
      <v-btn dark color="blue">
        <span>视频</span>
        <v-icon>ondemand_video</v-icon>
      </v-btn>

      <v-btn dark color="blue">
        <span>音乐</span>
        <v-icon>music_note</v-icon>
      </v-btn>

      <v-btn dark color="blue">
        <span>图书</span>
        <v-icon>book</v-icon>
      </v-btn>

      <v-btn dark color="blue">
        <span>图片</span>
        <v-icon>image</v-icon>
      </v-btn>
    </v-bottom-nav>
</template>

<script>
export default {
  name: 'MyFooter',
  data () {
    return {
      bottomNav: 3
    }
  },
  computed: {
    color () {
      switch (this.bottomNav) {
          case 0: return 'blue-grey'
          case 1: return 'teal'
          case 2: return 'brown'
          case 3: return 'indigo'
      }
    }
  }
}
</script>
