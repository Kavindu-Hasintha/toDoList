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
import CircularProgress from "@mui/material/CircularProgress";
import InputLabel from "@mui/material/InputLabel";
import Select from "@mui/material/Select";
import MenuItem from "@mui/material/MenuItem";
import FormControl from "@mui/material/FormControl";
import { SignUp } from "../utils/ApiFunctions";

const Signup = () => {
  const [data, setData] = useState({
    name: "",
    email: "",
    password: "",
    confirmPassword: "",
    userRole: "",
  });
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState("");
  const navigate = useNavigate();

  const handleSigninPage = () => {
    navigate("/signin");
  };

  const handleInputChange = (e) => {
    setData({ ...data, [e.target.name]: e.target.value });
  };

  const handleSignUp = async (e) => {
    e.preventDefault();
    setIsLoading(true);
    try {
      const response = await SignUp(data);
      console.log(response);
      if (response.status === 200) {
        navigate("/fp-otp");
      } else if (response.status === 400 || response.status === 500) {
        setError(response.data);
      } else {
        console.log(response.data);
        setError("An error occurred. Please try again later.");
      }
    } catch (error) {
      setError("An error occurred while signing in. Please try again.");
    } finally {
      setTimeout(() => {
        setError("");
        setIsLoading(false);
      }, 4000);
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
              name="name"
              value={data.name}
              onChange={handleInputChange}
              type="text"
            />
            <TextField
              label="Email"
              variant="outlined"
              name="email"
              value={data.email}
              onChange={handleInputChange}
              type="email"
            />
            <TextField
              label="Password"
              variant="outlined"
              name="password"
              value={data.password}
              onChange={handleInputChange}
              type="password"
            />
            <TextField
              label="Confirm Password"
              variant="outlined"
              name="confirmPassword"
              value={data.confirmPassword}
              onChange={handleInputChange}
              type="password"
            />
            <FormControl fullWidth>
              <InputLabel id="demo-simple-select-label">Role</InputLabel>
              <Select
                labelId="demo-simple-select-label"
                label="User Role"
                id="userRole"
                name="userRole"
                value={data.userRole}
                onChange={handleInputChange}
              >
                <MenuItem value={0}>Admin</MenuItem>
                <MenuItem value={1}>User</MenuItem>
              </Select>
            </FormControl>
          </Stack>
          <Stack spacing={2} direction={"column"} sx={{ marginTop: "25px" }}>
            {isLoading && (
              <Box sx={{ display: "flex", justifyContent: "center" }}>
                <CircularProgress />
              </Box>
            )}
            {!isLoading && (
              <Button variant="outlined" type="submit">
                Sign Up
              </Button>
            )}
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
