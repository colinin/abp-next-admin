import './vue'
import * as E from './events'

declare namespace VueEvents {
  export type VueEvents = E.VueEvents;
}

declare class VueEvents extends E.VueEvents {}

export default VueEvents
