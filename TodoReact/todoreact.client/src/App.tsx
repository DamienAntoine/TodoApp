import './App.css';
import { useState } from 'react';
import TodoList from './components/TodoList';
import FilterBar from "./components/FilterBar";
import LoginPage from './components/LoginPage';
import RegisterPage from './components/RegisterPage';
import AddTodoForm from './components/AddTodoForm';
import EditTodoForm from './components/EditTodoForm';
import type { Page } from './types/Page';
import { useTodoItems } from './hooks/useTodoItems';
import type { TodoItem } from './types/TodoItem';

export default function TodoApp()
{
	const [isAuthenticated, setIsAuthenticated] = useState(false);
	const [currentPage, setCurrentPage] = useState<Page>('login');
	const [showAddForm, setShowAddForm] = useState(false);
	const [editingTodo, setEditingTodo] = useState<TodoItem | null>(null);

	const [sortColumn, setSortColumn] = useState<number | null>(null);
	const [sortOrder, setSortOrder] = useState<'asc' | 'desc'>('asc');

	const { todos, loading, filterTodos, sortTodos, removeTodo, handleAddTodo, editTodo } = useTodoItems();

	function handleLogin() {
		setIsAuthenticated(true);
		setCurrentPage('todos');
	}

	function handleLogout() {
		setIsAuthenticated(false);
		setCurrentPage('login');
	}

	function handleSort(column: number, sortKey: string) {
		let newOrder: 'asc' | 'desc' = 'asc';
		if (sortColumn === column) {
			newOrder = sortOrder === 'asc' ? 'desc' : 'asc';
		}
		setSortColumn(column);
		setSortOrder(newOrder);
		sortTodos(sortKey, newOrder);
	}

	function handleEditTodo(todo: TodoItem) {
		setEditingTodo(todo);
	}

	async function handleSaveEdit(
		id: string,
		name: string,
		description: string,
		tag: string,
		deadLine: Date,
		isComplete: boolean,
		priority: number
	) {
		await editTodo(id, name, description, tag, deadLine, isComplete, priority);
		setEditingTodo(null);
	}

	if (loading) {
		return <div>Loading...</div>;
	}

	if (!isAuthenticated) {
		if (currentPage === 'login') {
			return (
				<LoginPage
					onLogin={handleLogin}
					onGoToRegister={() => setCurrentPage('register')}/>
			);
		}
		if (currentPage === 'register') {
			return (
				<RegisterPage
					onRegisterSuccess={() => setCurrentPage('login')}
					onGoToLogin={() => setCurrentPage('login')}
				/>
			);
		}
		
	}

	return (
		<div>
			<h1>Todo Manager</h1>
			<FilterBar onFilter={filterTodos} />
			<button onClick={handleLogout}>Logout</button>
			<button onClick={() => setShowAddForm(true)}>Add Todo</button>
			{showAddForm && (
				<AddTodoForm
					onAdd={async (name, description, tag, deadLine, priority) => {
						handleAddTodo(name, description, tag, deadLine, priority);
						setShowAddForm(false);
					}}
					onCancel={() => setShowAddForm(false)}
				/>
			)}
			{editingTodo && (
				<EditTodoForm
					todo={editingTodo}
					onSave={handleSaveEdit}
					onCancel={() => setEditingTodo(null)}
				/>
			)}
			<TodoList
				todos={todos}
				loading={loading}
				sortTodos={sortTodos}
				removeTodo={removeTodo}
				editTodo={handleEditTodo}
				sortColumn={sortColumn}
				sortOrder={sortOrder}
				onSort={handleSort}
			/>
		</div>
	);
}