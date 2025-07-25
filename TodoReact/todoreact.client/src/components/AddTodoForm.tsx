import React, { useState } from 'react';

type AddTodoFormProps = {
	onAdd: (name: string, description: string, tag: string, deadLine: Date, priority: number) => void;
	onCancel: () => void;
};

export default function AddTodoForm({ onAdd, onCancel }: AddTodoFormProps) {
	const [name, setName] = useState('');
	const [description, setDescription] = useState('');
	const [tag, setTag] = useState('');
	const [deadLine, setDeadLine] = useState('');
	const [priority, setPriority] = useState(1);

	function handleSubmit(e: React.FormEvent) {
		e.preventDefault();
		if (!name) return;
		onAdd(name, description, tag, new Date(deadLine), priority);
	}

	return (
		<form onSubmit={handleSubmit} style={{ marginBottom: 16 }}>
			<input
				type="text"
				placeholder="Name"
				value={name}
				onChange={e => setName(e.target.value)}
				required
			/>
			<input
				type="text"
				placeholder="Description"
				value={description}
				onChange={e => setDescription(e.target.value)}
			/>
			<input
				type="text"
				placeholder="Tag"
				value={tag}
				onChange={e => setTag(e.target.value)}
			/>
			<input
				type="date"
				value={deadLine}
				onChange={e => setDeadLine(e.target.value)}
			/>
			<select value={priority} onChange={e => setPriority(Number(e.target.value))}>
				<option value={1}>Low</option>
				<option value={2}>Medium</option>
				<option value={3}>High</option>
			</select>
			<button type="submit">Add</button>
			<button type="button" onClick={onCancel}>Cancel</button>
		</form>
	);
}