import { useEffect, useState } from 'react';
import type { TodoItem } from '../types/TodoItem';

type QueryParams = {
	tag?: string;
	keyword?: string;
	priority?: number;
	isComplete?: boolean | "";
	sortBy?: string;
	order?: 'asc' | 'desc';
};

export function useTodoItems() {
	const [todos, setTodos] = useState<TodoItem[]>([]);
	const [loading, setLoading] = useState(true);
	const [error, setError] = useState("");
	const [query, setQuery] = useState<QueryParams>({});

	async function fetchTodos() {
		setLoading(true);
		const params = new URLSearchParams();
		if (query.tag) params.append("tag", query.tag);
		if (query.keyword) params.append("keyword", query.keyword);
		if (query.priority && query.priority > 0) params.append("priority", query.priority.toString());
		if (query.isComplete !== undefined && query.isComplete !== "") params.append("isComplete", query.isComplete.toString());
		if (query.sortBy) params.append("sortBy", query.sortBy);
		if (query.order) params.append("order", query.order);

		const response = await fetch(`/api/TodoItems?${params.toString()}`);
		if (response.ok) {
			const data = await response.json();
			setTodos(data);
		}
		setLoading(false);
	}

	useEffect(() => {
		fetchTodos();
	// eslint-disable-next-line react-hooks/exhaustive-deps
	}, [query]);

	async function handleAddTodo(name: string, description: string, tag: string, deadLine: Date, priority: number) {
		if (priority < 1 || priority > 3) {
			setError("Priority must be between 1 (Low) and 3 (High)");
			return;
		}
		await addTodo(name, description, tag, deadLine, priority);
	}

	async function addTodo(name: string, description: string, tag: string, deadLine: Date, priority: number) {
		const response = await fetch('/api/TodoItems', {
			method: 'POST',
			credentials: 'include',
			headers: {
				'Content-Type': 'application/json'
			},
			body: JSON.stringify({
				name,
				description,
				tag,
				deadLine: deadLine.toISOString(),
				isComplete: false,
				priority
			})
		});
		if (response.ok) {
			await fetchTodos();
		} else {
			const errorText = await response.text();
			console.error("AddTodo error:", errorText);
			setError(errorText);
		}
	}

	async function editTodo(
		id: string,
		name: string,
		description: string,
		tag: string,
		deadLine: Date,
		isComplete: boolean,
		priority: number
	) {
		if (priority < 1 || priority > 3) {
			setError("Priority must be between 1 (Low) and 3 (High)");
			return;
		}

		const response = await fetch(`/api/TodoItems/${id}`, {
			method: 'PUT',
			credentials: 'include',
			headers: {
				'Content-Type': 'application/json'
			},
			body: JSON.stringify({
				id,
				name,
				description,
				tag,
				deadLine: deadLine.toISOString(),
				isComplete,
				priority
			})
		});
		if (response.ok) {
			await fetchTodos();
		} else {
			const errorText = await response.text();
			console.error("EditTodo error:", errorText);
			setError(errorText);
		}
	}

	async function removeTodo(id: string) {
		const response = await fetch(`/api/TodoItems/${id}`, {
			method: 'DELETE',
		});
		if (response.ok) {
			setTodos(todos => todos.filter(todo => todo.id !== id));
		}
	}

	function filterTodos(tag: string, keyword: string, priority: number, isComplete: boolean | "") {
		setQuery(q => ({
			...q,
			tag,
			keyword,
			priority,
			isComplete
		}));
	}

	function sortTodos(sortBy: string, order: 'asc' | 'desc' = 'asc') {
		setQuery(q => ({
			...q,
			sortBy,
			order
		}));
	}

	return { todos, loading, sortTodos, filterTodos, removeTodo, handleAddTodo, editTodo, error };
}

