export interface TodoItem {
	id: string;
	name: string;
	isComplete: boolean;
	description?: string;
	tag?: string;
	createdAt?: string;
	updatedAt?: string;
	deadLine?: string;
	priority: number;
}