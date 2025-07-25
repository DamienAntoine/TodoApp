import { useState } from "react";

type LoginPageProps = {
	onLogin: () => void;
	onGoToRegister: () => void;
};

export default function LoginPage(props: LoginPageProps) {
	const onLogin = props.onLogin;
	const onGoToRegister = props.onGoToRegister;

	const [username, setUsername] = useState("");
	const [password, setPassword] = useState("");
	const [error, setError] = useState<string | null>(null);

	async function handleSubmit(event: React.FormEvent) {
		event.preventDefault();

		const response = await fetch('/api/Account/login', {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json'
			},
			body: JSON.stringify({
				username,
				password
			})
		});

		if (response.ok) {
			onLogin();
		} else {
			setError("Incorrect Username or Password");
		}
	}

	return (
		<div>
			<form onSubmit={handleSubmit}>
				<input
					type="text"
					placeholder="Username"
					value={username}
					onChange={e => setUsername(e.target.value)}
				/>
				<input
					type="password"
					placeholder="Password"
					value={password}
					onChange={e => setPassword(e.target.value)}
				/>
				<button type="submit">Login</button>
			</form>
			{error && <div style={{ color: "red" }}>{error}</div>}
			<button onClick={onGoToRegister}>Register</button>
		</div>
	);
}