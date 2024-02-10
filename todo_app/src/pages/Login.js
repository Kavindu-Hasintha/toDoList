import React, { useState } from "react";
import TextField from "@mui/material/TextField";
import LoginStyles from "../Styles/Login.module.css";
import Header from "../components/Header";
import Footer from "../components/Footer";
import { useNavigate } from "react-router-dom";
// import axios from "axios";
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import useHttp from "../hooks/use-http";

const Login = () => {
  const [login, setLogin] = useState({
    email: "",
    password: "",
  });
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState("");
  const navigate = useNavigate();

  const handleSignupPage = () => {
    navigate("/signup");
  };

  const handleInputChange = (e) => {
    setLogin({ ...login, [e.target.name]: e.target.value });
  };

  // const { isLoading, error, sendRequest: sendLoginRequest } = useHttp();

  const checkUser = (id) => {
    if (id === 1) {
      navigate(id + "/admindashboard");
    } else {
      navigate(id + "/home");
    }
    toast.success("Login Success.");
  };

  const handleSignIn = async () => {
    await sendLoginRequest(
      {
        url: "https://localhost:7068/api/Users/getUserId",
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        data: login,
      },
      checkUser
    );

    // if (userId === 1) {
    //   navigate(userId + "/admindashboard");
    // } else {
    //   navigate(userId + "/home");
    // }
    // toast.success("Login Success.");

    // axios
    //   .post("https://localhost:7068/api/Users/getUserId", login)
    //   .then((res) => {
    //     if (res.data === 1) {
    //       navigate(res.data + "/admindashboard");
    //     } else {
    //       navigate(res.data + "/home");
    //     }
    //     toast.success("Login Success.");
    //   })
    //   .catch((err) => {
    //     // toast.error(err.response.data.LoginError.errors[0].errorMessage);
    //     toast.error("Something went wrong. Try again later.");
    //   });
  };

  return (
    <div className={LoginStyles.backgroundImage}>
      {isLoading && <div>Loading...</div>}
      {!isLoading && (
        <div>
          <Header name="" onHandleTopButton={handleSignupPage} pageId="0" />
          <div className="container mt-4 shadow p-4 mb-4 bg-white rounded">
            <div className="row">
              <div className="col-8">
                <img src="https://picsum.photos/id/155/850/500" alt="login" />
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
                  <button
                    className="btn btn-primary py-2 fs-5"
                    onClick={handleSignIn}
                  >
                    Sign In
                  </button>
                </div>
                <div className="row my-4 d-flex text-start">
                  <a href="#">Forgot your password?</a>
                </div>
              </div>
            </div>
          </div>
          <Footer />
        </div>
      )}
    </div>
  );
};

export default Login;
