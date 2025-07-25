import { useState } from 'react';
import type { TodoItem } from '../types/TodoItem';

interface EditTodoFormProps {
	todo: TodoItem;
	onSave: (
		id: string,
		name: string,
		description: string,
		tag: string,
		deadLine: Date,
		isComplete: boolean,
		priority: number
	) => void;
	onCancel: () => void;
}

export default function EditTodoForm({ todo, onSave, onCancel }: EditTodoFormProps) {
	const [name, setName] = useState(todo.name);
	const [description, setDescription] = useState(todo.description || '');
	const [tag, setTag] = useState(todo.tag || '');
	const [deadLine, setDeadLine] = useState(
		todo.deadLine ? new Date(todo.deadLine).toISOString().slice(0, 16) : ''
	);
	const [isComplete, setIsComplete] = useState(todo.isComplete);
	const [priority, setPriority] = useState(todo.priority);

	function handleSubmit(e: React.FormEvent) {
		e.preventDefault();

		if (!name.trim()) {
			alert('Name is required');
			return;
		}

		const deadLineDate = deadLine ? new Date(deadLine) : new Date();

		onSave(
			todo.id,
			name.trim(),
			description.trim(),
			tag.trim(),
			deadLineDate,
			isComplete,
			priority
		);
	}

	return (
		<div style={{
			position: 'fixed',
			top: 0,
			left: 0,
			right: 0,
			bottom: 0,
			backgroundColor: 'rgba(0,0,0,0.5)',
			display: 'flex',
			justifyContent: 'center',
			alignItems: 'center'
		}}>
			<div style={{
				backgroundColor: 'darkgrey',
				padding: '20px',
				borderRadius: '8px',
				minWidth: '400px'
			}}>
				<h3>Edit Todo</h3>
				<form onSubmit={handleSubmit}>
					<div style={{ marginBottom: '10px' }}>
						<label>Name:</label>
						<input
							type="text"
							value={name}
							onChange={(e) => setName(e.target.value)}
							required
							style={{ width: '100%', padding: '5px' }}
						/>
					</div>

					<div style={{ marginBottom: '10px' }}>
						<label>Description:</label>
						<textarea
							value={description}
							onChange={(e) => setDescription(e.target.value)}
							style={{ width: '100%', padding: '5px', minHeight: '60px' }}
						/>
					</div>

					<div style={{ marginBottom: '10px' }}>
						<label>Tag:</label>
						<input
							type="text"
							value={tag}
							onChange={(e) => setTag(e.target.value)}
							style={{ width: '100%', padding: '5px' }}
						/>
					</div>

					<div style={{ marginBottom: '10px' }}>
						<label>Deadline:</label>
						<input
							type="datetime-local"
							value={deadLine}
							onChange={(e) => setDeadLine(e.target.value)}
							style={{ width: '100%', padding: '5px' }}
						/>
					</div>

					<div style={{ marginBottom: '10px' }}>
						<label>Priority:</label>
						<select
							value={priority}
							onChange={(e) => setPriority(Number(e.target.value))}
							style={{ width: '100%', padding: '5px' }}
						>
							<option value={1}>Low (1)</option>
							<option value={2}>Medium (2)</option>
							<option value={3}>High (3)</option>
						</select>
					</div>

					<div style={{ marginBottom: '10px' }}>
						<label>
							<input
								type="checkbox"
								checked={isComplete}
								onChange={(e) => setIsComplete(e.target.checked)}
							/>
							Completed
						</label>
					</div>

					<div style={{ display: 'flex', gap: '10px', justifyContent: 'flex-end' }}>
						<button type="button" onClick={onCancel}>Cancel</button>
						<button type="submit">Save Changes</button>
					</div>
				</form>
			</div>
		</div>
	);
}