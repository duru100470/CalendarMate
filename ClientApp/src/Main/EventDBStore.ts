import { writable } from "svelte/store";

type event = {
    eventId: number,
    title: string,
    date: Date,
    description: string,
    userId: number}

const eventDBStore = writable<event[]>([]);

export { eventDBStore };
export type { event };