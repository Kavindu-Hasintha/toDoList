import React, { useState } from "react";
import TextField from "@mui/material/TextField";
import LoginStyles from "../Styles/Login.module.css";
import Header from "./Header";
import Footer from "./Footer";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import axios from "axios";

const Signup = () => {
  const [data, setData] = useState({
    name: "",
    email: "",
    password: "",
  });
  const [rePass, setRePass] = useState("");
  const navigate = useNavigate();

  const handleSigninPage = () => {
    navigate("/");
  };

  const handleChange = (e) => {
    setData({ ...data, [e.target.name]: e.target.value });
  };

  const rePassHandleChange = (e) => {
    setRePass(e.target.value);
  };

  const handleSignUp = () => {
    if (data.password !== rePass) {
      toast.error("Passwords aren't matching.");
    } else {
      axios
        .post("https://localhost:7068/api/Users", data)
        .then((res) => {
          toast.success(res.data);
          navigate("/");
        })
        .catch((err) => {
          toast.error(err.response.data.UserError.errors[0].errorMessage);
        });
    }
  };

  return (
    <div className={LoginStyles.backgroundImage}>
      <Header name="" onHandleTopButton={handleSigninPage} pageId="1" />
      <div className="container mt-4 shadow p-4 mb-4 bg-white rounded">
        <div className="row">
          <div className="col-8">
            <img src="https://picsum.photos/id/198/850/500" alt="photo" />
          </div>
          <div className="col-4 bg-white d-inline px-5">
            <div className="row text-start mb-4">
              <h2>Sign Up</h2>
            </div>
            <div className="row my-4">
              <TextField
                id="outlined-basic"
                label="Name"
                variant="outlined"
                name="name"
                onChange={handleChange}
              />
            </div>
            <div className="row my-4">
              <TextField
                id="outlined-basic"
                label="Email"
                variant="outlined"
                name="email"
                onChange={handleChange}
              />
            </div>
            <div className="row my-4">
              <TextField
                id="outlined-password-input"
                label="Password"
                type="password"
                name="password"
                onChange={handleChange}
              />
            </div>
            <div className="row my-4">
              <TextField
                id="outlined-password-input"
                label="Re-Password"
                type="password"
                name="rePass"
                onChange={rePassHandleChange}
              />
            </div>
            <div className="row my-4">
              <button
                className="btn btn-primary py-2 fs-5"
                onClick={handleSignUp}
              >
                Sign Up
              </button>
            </div>
          </div>
        </div>
      </div>
      <Footer />
    </div>
  );
};

export default Signup;
