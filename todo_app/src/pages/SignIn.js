import React, { useState } from "react";
import Box from "@mui/material/Box";
import TextField from "@mui/material/TextField";
import LoginStyles from "../Styles/Login.module.css";
import Header from "../components/Header";
import Footer from "../components/Footer";
import { useNavigate } from "react-router-dom";
// import axios from "axios";
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import useHttp from "../hooks/use-http";
import { SignIn } from "../utils/ApiFunctions";
import { FilePresent } from "@mui/icons-material";
// import { Button } from "react-bootstrap";
import Button from "@mui/material/Button";
import Stack from "@mui/material/Stack";
import CircularProgress from "@mui/material/CircularProgress";
import { Link } from "react-router-dom";
import { Typography } from "@mui/material";

const Signin = () => {
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

  const handleForgotPassword = () => {
    navigate("/forget-password");
  };

  const handleInputChange = (e) => {
    setLogin({ ...login, [e.target.name]: e.target.value });
  };

  // const { isLoading, error, sendRequest: sendLoginRequest } = useHttp();

  const checkUser = (id) => {
    if (id === 0) {
      navigate("/admindashboard");
    } else {
      navigate("/home");
    }
    toast.success("Login Success.");
  };

  const handleSignIn = async (e) => {
    e.preventDefault();
    setIsLoading(true);
    const response = await SignIn(login);
    if (response.status === 200) {
      const token = response.data.token;
      const refreshToken = response.data.refreshToken;
      const userRole = response.data.userRole;

      localStorage.setItem("token", token);
      localStorage.setItem("refreshToken", refreshToken);
      localStorage.setItem("userRole", userRole);

      checkUser(userRole);
    } else {
      setError("Invalid username or password. Please try again.");
    }
    setTimeout(() => {
      setError("");
      setIsLoading(false);
    }, 4000);
    // await sendLoginRequest(
    //   {
    //     url: "https://localhost:7068/api/Users/getUserId",
    //     method: "POST",
    //     headers: {
    //       "Content-Type": "application/json",
    //     },
    //     data: login,
    //   },
    //   checkUser
    // );
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

  // return (
  //   <div className={LoginStyles.backgroundImage}>
  //     <div>
  //       <Header name="" onHandleTopButton={handleSignupPage} pageId="0" />
  //       <div className="container mt-4 shadow p-4 mb-4 bg-white rounded">
  //         <div className="row">
  //           <div className="col-8">
  //             <img src="https://picsum.photos/id/155/850/500" alt="login" />
  //           </div>
  //           {isLoading && !error && <div>Loading...</div>}
  //           {!isLoading && error && <div className="text-danger">{error}</div>}
  //           {!isLoading && !error && (
  //             <div className="col-4 bg-white d-inline px-5">
  //               <div className="row text-start mb-4">
  //                 <h2>Sign In</h2>
  //               </div>
  //               <div className="row my-4">
  //                 <TextField
  //                   id="outlined-basic"
  //                   label="Username"
  //                   variant="outlined"
  //                   name="email"
  //                   onChange={handleInputChange}
  //                 />
  //               </div>
  //               <div className="row my-4">
  //                 <TextField
  //                   id="outlined-password-input"
  //                   label="Password"
  //                   type="password"
  //                   name="password"
  //                   onChange={handleInputChange}
  //                 />
  //               </div>
  //               <div className="row my-4">
  //                 <button
  //                   className="btn btn-primary py-2 fs-5"
  //                   onClick={handleSignIn}
  //                 >
  //                   Sign In
  //                 </button>
  //               </div>
  //               <div className="row my-4 d-flex text-start">
  //                 <a href="#">Forgot your password?</a>
  //               </div>
  //             </div>
  //           )}
  //         </div>
  //       </div>
  //       <Footer />
  //     </div>
  //   </div>
  // );

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
        <form onSubmit={handleSignIn}>
          <Stack spacing={2} direction={"column"}>
            <TextField
              label="Username"
              variant="outlined"
              // fullWidth
              name="email"
              value={login.email}
              onChange={handleInputChange}
              type="email"
            />
            <TextField
              label="Password"
              variant="outlined"
              // fullWidth
              name="password"
              value={login.password}
              onChange={handleInputChange}
              type="password"
            />
          </Stack>
          <Stack spacing={2} direction={"column"} sx={{ marginTop: "25px" }}>
            {isLoading && (
              <Box sx={{ display: "flex", justifyContent: "center" }}>
                <CircularProgress />
              </Box>
            )}
            {!isLoading && (
              <Button variant="outlined" type="submit">
                Sign In
              </Button>
            )}
            <Link to="/forget-password" style={{ textDecoration: "none" }}>
              <Typography sx={{ color: "#1976d2" }}>
                Forgot Password?
              </Typography>
            </Link>
          </Stack>
        </form>
        <Stack
          spacing={2}
          direction={"row"}
          sx={{ justifyContent: "center", marginTop: "10px" }}
        >
          <p>Don't have an account?</p>
          <Button variant="text" onClick={handleSignupPage}>
            Sign Up
          </Button>
        </Stack>
      </Box>
    </Box>
  );
};

export default Signin;
