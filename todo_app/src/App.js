import "./App.css";
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import { ToastContainer } from "react-toastify";
import Login from "./pages/Login";
import Signup from "./components/SignUp";
import Home from "./components/Home";
import AdminDashboard from "./components/AdminDashboard";
import Welcome from "./pages/Welcome";

function App() {
  return (
    <div className="App">
      <ToastContainer position="top-center" />
      <BrowserRouter>
        <Routes>
          <Route path="/welcome" element={<Welcome />} />
          <Route path="/login" element={<Login />} />
          <Route path="/signup" element={<Signup />} />
          <Route path=":id/home/*" element={<Home />} />
          <Route path=":id/admindashboard/*" element={<AdminDashboard />} />
          <Route path="*" element={<Navigate to="/welcome" replace />} />
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
