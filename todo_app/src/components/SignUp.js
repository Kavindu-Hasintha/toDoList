import React, { useState } from "react";
import TextField from "@mui/material/TextField";
import LoginStyles from "../Styles/Login.module.css";
import Header from "./Header";
import Footer from "./Footer";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import axios from "axios";

import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import Stack from "@mui/material/Stack";
import Link from "@mui/material/Link";

const Signup = () => {
  const [data, setData] = useState({
    name: "",
    email: "",
    password: "",
    confirmPassword: "",
  });
  const [rePass, setRePass] = useState("");
  const navigate = useNavigate();

  const handleSigninPage = () => {
    navigate("/signin");
  };

  const handleInputChange = (e) => {
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

  /*
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
  */
  return (
    <Box
      sx={{
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
      }}
    >
      <Box
        sx={{
          width: "300px",
          margin: "auto",
          marginTop: "100px",
          padding: "30px",
          boxShadow: 3,
          borderRadius: "16px",
        }}
      >
        <form onSubmit={handleSignUp}>
          <Stack spacing={2} direction={"column"}>
            <TextField
              label="Full Name"
              variant="outlined"
              // fullWidth
              name="name"
              value={data.name}
              onChange={handleInputChange}
              type="text"
            />
            <TextField
              label="Email"
              variant="outlined"
              // fullWidth
              name="email"
              value={data.email}
              onChange={handleInputChange}
              type="email"
            />
            <TextField
              label="Password"
              variant="outlined"
              // fullWidth
              name="password"
              value={data.password}
              onChange={handleInputChange}
              type="password"
            />
            <TextField
              label="Confirm Password"
              variant="outlined"
              // fullWidth
              name="confirmPassword"
              value={data.confirmPassword}
              onChange={handleInputChange}
              type="password"
            />
          </Stack>
          <Stack spacing={2} direction={"column"} sx={{ marginTop: "25px" }}>
            <Button variant="outlined" type="submit">
              Sign Up
            </Button>
          </Stack>
        </form>
        <Stack
          spacing={2}
          direction={"row"}
          sx={{ justifyContent: "center", marginTop: "10px" }}
        >
          <p>Already have an account?</p>
          <Button variant="text" onClick={handleSigninPage}>
            Sign In
          </Button>
        </Stack>
      </Box>
    </Box>
  );
};

export default Signup;
