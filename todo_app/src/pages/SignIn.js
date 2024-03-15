import React, { useState } from "react";
import Box from "@mui/material/Box";
import TextField from "@mui/material/TextField";
import { useNavigate } from "react-router-dom";
import { SignIn } from "../utils/ApiFunctions";
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

  const handleInputChange = (e) => {
    setLogin({ ...login, [e.target.name]: e.target.value });
  };

  const checkUser = (userRole) => {
    switch (userRole) {
      case 0:
        navigate("/admindashboard");
        break;
      case 1:
        navigate("/home");
        break;
      default:
        navigate("/welcome");
        break;
    }
  };

  const handleSignIn = async (e) => {
    e.preventDefault();
    setIsLoading(true);

    try {
      const response = await SignIn(login);
      if (response.status === 200) {
        const { token, refreshToken, userRole } = response.data;

        localStorage.setItem("token", token);
        localStorage.setItem("refreshToken", refreshToken);
        localStorage.setItem("userRole", userRole);

        checkUser(userRole);
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
        {error && (
          <Typography
            color="red"
            sx={{ textAlign: "left", marginBottom: "8px", fontSize: "0.85rem" }}
          >
            {error}
          </Typography>
        )}
        <form onSubmit={handleSignIn}>
          <Stack spacing={2} direction={"column"}>
            <TextField
              label="Username"
              variant="outlined"
              name="email"
              value={login.email}
              onChange={handleInputChange}
              type="email"
            />
            <TextField
              label="Password"
              variant="outlined"
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
