import React, { useState, useRef, useEffect } from "react";

interface FilterBarProps {
	onFilter: (tag: string, keyword: string, priority: number, isComplete: boolean | "") => void;
}

const FilterBar: React.FC<FilterBarProps> = ({ onFilter }) => {
	const [tag, setTag] = useState("");
	const [keyword, setKeyword] = useState("");
	const [priority, setPriority] = useState<number | "">("");
	const [isComplete, setIsComplete] = useState<boolean | "">("");

	const isCompleteRef = useRef<HTMLInputElement>(null);

	useEffect(() => {
		if (isCompleteRef.current) {
			isCompleteRef.current.indeterminate = isComplete === "";
		}
	}, [isComplete]);

	function handleIsCompleteClick() {
		if (isComplete === "") {
			setIsComplete(true);
		}
		else if (isComplete === true) {
			setIsComplete(false);
		}
		else {
			setIsComplete("");
		}
	}

	function handleFilter(e: React.FormEvent) {
		e.preventDefault();
		onFilter(
			tag,
			keyword,
			priority === "" ? 0 : Number(priority),
			isComplete
		);
	}

	return (
		<form onSubmit={handleFilter} style={{ marginBottom: "1em" }}>
			<input
				type="text"
				placeholder="Keyword"
				value={keyword}
				onChange={e => setKeyword(e.target.value)}
			/>
			<input
				type="text"
				placeholder="Tag"
				value={tag}
				onChange={e => setTag(e.target.value)}
			/>
			<select
				value={priority}
				onChange={e => setPriority(e.target.value === "" ? "" : Number(e.target.value))}
			>
				<option value="">Priority</option>
				<option value="1">Low</option>
				<option value="2">Medium</option>
				<option value="3">High</option>
			</select>
			<label style={{ userSelect: "none", cursor: "pointer", marginLeft: "0.5em" }}>
				Completed
				<input
					type="checkbox"
					ref={isCompleteRef}
					checked={isComplete === true}
					onChange={handleIsCompleteClick}
					style={{ marginLeft: "0.5em" }}
				/>
				<span style={{ marginLeft: "0.5em" }}>
					{isComplete === "" ? "(Any)" : isComplete ? "(Yes)" : "(No)"}
				</span>
			</label>
			<button type="submit">Filter</button>
		</form>
	);
};

export default FilterBar;