import React from 'react';

interface EditButtonProps {
  onClick: () => void;
  disabled?: boolean;
}

const EditButton: React.FC<EditButtonProps> = ({ onClick, disabled = false }) => {
  return (
    <button
      type="button"
      onClick={onClick}
      disabled={disabled}
      className="edit-button"
      aria-label="Edit"
    >
      Edit
    </button>
  );
};

export default EditButton;