import "./App.css";
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import { ToastContainer } from "react-toastify";
import SignIn from "./pages/SignIn";
import Signup from "./components/SignUp";
import Home from "./components/Home";
import AdminDashboard from "./components/AdminDashboard";
import Welcome from "./pages/Welcome";
import FPEmail from "./components/FPEmail";
import FPOTP from "./components/FPOTP";
import FPNewPassword from "./components/FPNewPassword";

function App() {
  return (
    <div className="App">
      <ToastContainer position="top-center" />
      <BrowserRouter>
        <Routes>
          <Route path="/welcome" element={<Welcome />} />
          <Route path="/signin" element={<SignIn />} />
          <Route path="/signup" element={<Signup />} />
          <Route path="/forget-password" element={<FPEmail />} />
          <Route path="/fp-otp" element={<FPOTP />} />
          <Route path="/fp-new-password" element={<FPNewPassword />} />
          <Route path="/home/*" element={<Home />} />
          <Route path="/admindashboard/*" element={<AdminDashboard />} />
          <Route path="*" element={<Navigate to="/welcome" replace />} />
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
