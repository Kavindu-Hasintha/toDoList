import React from "react";
import TextField from "@mui/material/TextField";
import LoginStyles from "../Styles/Login.module.css";
import Header from "./Header";
import Footer from "./Footer";
import { useNavigate } from "react-router-dom";

const Login = () => {
  const navigate = useNavigate();

  const handleSignupPage = () => {
    navigate("/signup");
  };

  return (
    <div className={LoginStyles.backgroundImage}>
      <Header name="" onHandleTopButton={handleSignupPage} pageId="0" />
      <div className="container mt-4 shadow p-4 mb-4 bg-white rounded">
        <div className="row">
          <div className="col-8">
            <img src="https://picsum.photos/id/155/850/500" alt="photo" />
          </div>
          <div className="col-4 bg-white d-inline px-5">
            <div className="row text-start mb-4">
              <h2>Sign In</h2>
            </div>
            <div className="row my-4">
              <TextField
                id="outlined-basic"
                label="Username"
                variant="outlined"
              />
            </div>
            <div className="row my-4">
              <TextField
                id="outlined-password-input"
                label="Password"
                type="password"
              />
            </div>
            <div className="row my-4">
              <button className="btn btn-primary py-2 fs-5">Sign In</button>
            </div>
            <div className="row my-4 d-flex text-start">
              <a href="#">Forgot your password?</a>
            </div>
          </div>
        </div>
      </div>
      <Footer />
    </div>
  );
};

export default Login;
