import * as React from 'react';
import { SortKeys } from '../types/SortKeys';
import type { TodoItem } from '../types/TodoItem';
import EditButton from '../components/EditButton';

type HeaderNameProps = {
	name: string;
	onNameClick: () => void;
};

function HeaderName({ name, onNameClick }: HeaderNameProps) {
	return (
		<th>
			<button onClick={onNameClick}>
				{name}
			</button >
		</th>
	);
}

type TodoListProps = {
	todos: TodoItem[];
	loading: boolean;
	sortTodos: (sortBy: string, order: 'asc' | 'desc') => void;
	removeTodo: (id: string) => void;
	editTodo: (todo: TodoItem) => void;
	sortColumn: number | null;
	sortOrder: 'asc' | 'desc';
	onSort: (column: number, sortKey: string) => void;
};

const TodoList: React.FC<TodoListProps> = ({
	todos,
	loading,
	removeTodo,
	onSort,
	editTodo
}) => {
	if (loading) {
		return <div>Loading...</div>;
	}

	if (todos.length === 0) {
		return <div>No todo items found.</div>;
	}

	function handleClick(column: number) {
		const sortKey = SortKeys[column];
		if (!sortKey) {
			return;
		}
		onSort(column, sortKey);
	}

	return (
		<>
			<div>
				<h2>Todo List</h2>
			</div>
			<div>
				<table>
					<thead>
						<tr>
							<HeaderName name={"Name"} onNameClick={() => handleClick(0)} />
							<HeaderName name={"Description"} onNameClick={() => handleClick(1)} />
							<HeaderName name={"Priority"} onNameClick={() => handleClick(2)} />
							<HeaderName name={"Done"} onNameClick={() => handleClick(3)} />
							<HeaderName name={"Deadline"} onNameClick={() => handleClick(4)} />
							<HeaderName name={"Creation date"} onNameClick={() => handleClick(5)} />
							<HeaderName name={"Last update"} onNameClick={() => handleClick(6)} />
						</tr>
					</thead>
					<tbody>
						{todos.map(todo => (
							<tr key={todo.id}>
								<td>{todo.name}</td>
								<td>{todo.description}</td>
								<td>{todo.priority}</td>
								<td>{todo.isComplete ? "Yes" : "No"}</td>
								<td>{todo.deadLine ? new Date(todo.deadLine).toLocaleDateString() : ""}</td>
								<td>{todo.createdAt ? new Date(todo.createdAt).toLocaleDateString() : ""}</td>
								<td>{todo.updatedAt ? new Date(todo.updatedAt).toLocaleDateString() : ""}</td>
								<td>
									<EditButton
										onClick={() => {
											console.log('Edit', todo.id);
											editTodo(todo); // Call editTodo with the entire todo object
										}}
									/>
									<button
										onClick={() => removeTodo(todo.id)}
										style={{
											background: 'none',
											border: 'none',
											color: 'red',
											fontSize: '1.2em',
											cursor: 'pointer'
										}}
										aria-label="Delete"
										title="Delete"
									>
										&#10005;
									</button>
								</td>
							</tr>
						))}
					</tbody>
				</table>
			</div>
		</>
	);
};

export default TodoList;