import { useState } from "react";

type RegisterPageProps = {
	onRegisterSuccess: () => void;
	onGoToLogin: () => void;
}

export default function RegisterPage(props: RegisterPageProps) {
	const onRegisterSuccess = props.onRegisterSuccess;
	const onGoToLogin = props.onGoToLogin;

	const [username, setUsername] = useState("");
	const [password, setPassword] = useState("");
	const [error, setError] = useState<string | null>(null);

	async function handleSubmit(event: React.FormEvent) {
		event.preventDefault();

		const response = await fetch('/api/Account/register', {
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
			setError(null);
			onRegisterSuccess();
		} else {
			setError("Your password is not strong enough (example: Aa123!");
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
				<button type="submit">Register</button>
			</form>
			{error && <div style={{ color: "red" }}>{error}</div>}
			<button onClick={onGoToLogin}>Login</button>
		</div>
	);
}