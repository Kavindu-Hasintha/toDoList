import "./App.css";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import { ToastContainer } from "react-toastify";
import Login from "./components/Login";
import Signup from "./components/SignUp";
import Home from "./components/Home";
import AdminDashboard from "./components/AdminDashboard";

function App() {
  return (
    <div className="App">
      <ToastContainer position="top-center" />
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<Login />} />
          <Route path="/signup" element={<Signup />} />
          <Route path=":id/home" element={<Home />} />
          <Route path=":id/admindashboard" element={<AdminDashboard />} />
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
