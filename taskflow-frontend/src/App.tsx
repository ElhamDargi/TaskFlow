import { BrowserRouter, Routes, Route } from "react-router-dom";
import CreateProjectForm from "./components/CreateProjectForm";
import ProjectList from "./components/ProjectList";
import ProjectDetails from "./components/ProjectDetails";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route
          path="/projects"
          element={
            <>
              <CreateProjectForm />
              <ProjectList />
            </>
          }
        />
        <Route path="/projects/:projectId" element={<ProjectDetails />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
