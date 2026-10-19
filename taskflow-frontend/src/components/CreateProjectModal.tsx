import CreateProjectForm from "./CreateProjectForm";

type CreateProjectModalProps = {
  onClose: () => void;
};

function CreateProjectModal({ onClose }: CreateProjectModalProps) {
  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/50 p-4">
      <div
        role="dialog"
        aria-modal="true"
        aria-labelledby="create-project-title"
        className="w-full max-w-lg rounded-xl bg-white p-6 shadow-xl"
      >
        <div className="mb-6 flex items-center justify-between">
          <h2
            id="create-project-title"
            className="text-xl font-semibold text-gray-900"
          >
            Create New Project
          </h2>

          <button
            type="button"
            onClick={onClose}
            aria-label="Close modal"
            className="text-2xl text-gray-500 hover:text-gray-900"
          >
            &times;
          </button>
        </div>

        <CreateProjectForm  onClose={onClose} />

        <div className="mt-6 flex justify-end">
          <button
            type="button"
            onClick={onClose}
            className="rounded-lg border border-gray-300 px-4 py-2 text-gray-700 hover:bg-gray-100"
          >
            Cancel
          </button>
        </div>
      </div>
    </div>
  );
}

export default CreateProjectModal;
