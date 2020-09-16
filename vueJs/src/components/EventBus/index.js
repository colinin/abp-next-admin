/**
 * vue-event-handler typescript adapter
 * src: https://github.com/sandeepk01/vue-event-handler
 * reference: https://github.com/cklmercer/vue-events
 */

function plugin(Vue) {
  if (plugin.installed) {
    return
  }
  const evmap = {}
  const events = new Vue({
    methods: {
      fire(name, data = null) {
        this.emit(name, data)
      },
      emit(name, data = null) {
        this.$emit(name, data)
      },
      listen(name, cb) {
        this.on(name, cb)
      },
      listenOnce(name, cb) {
        this.once(name, cb)
      },
      on(name, cb) {
        evmap[name] = cb
        this.$on(name, cb)
      },
      once(name, cb) {
        evmap[name] = cb
        this.$once(name, cb)
      },
      off(name, cb) {
        if (!cb) {
          cb = evmap[name]
        }
        this.$off(name, cb)
      },
      unlisten(name, cb) {
        this.off(name, cb)
      },
      removeAll() {
        this.$off()
      }
    }
  })

  Vue.mixin({
    beforeCreate() {
      if (typeof this.$options.events !== 'object') return
      let eventMap = {}
      for (const key in this.$options.events) {
        eventMap[key] = this.$options.events[key].bind(this)
      }
      this.$once('hook:beforeMount', () => {
        for (const key in eventMap) {
          events.$on(key, eventMap[key])
        }
      })
      this.$once('hook:beforeDestroy', () => {
        for (const key in eventMap) {
          events.$off(key, eventMap[key])
        }
        eventMap = null
      })
    }
  })

  Vue.events = events

  Object.defineProperty(Vue.prototype, '$events', {
    get() {
      return events
    }
  })
}

if (typeof window !== 'undefined' && window.Vue) {
  window.Vue.use(plugin)
}

export default plugin
