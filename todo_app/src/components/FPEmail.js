import React, { useState } from "react";
import Box from "@mui/material/Box";
import TextField from "@mui/material/TextField";
import Button from "@mui/material/Button";
import Stack from "@mui/material/Stack";
import { Link } from "react-router-dom";
import CircularProgress from "@mui/material/CircularProgress";
import { useNavigate } from "react-router-dom";
import { Typography } from "@mui/material";

const FPEmail = () => {
  const [email, setEmail] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();

  const handleInputChange = (e) => {
    setEmail(e.target.value);
  };

  const handleSendOTP = (e) => {
    e.preventDefault();
    setIsLoading(true);
    navigate("/fp-otp");
    setIsLoading(false);
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
        <Stack spacing={2} direction={"column"}>
          <TextField
            label="Email"
            variant="outlined"
            // fullWidth
            name="email"
            value={email}
            onChange={handleInputChange}
            type="email"
          />
        </Stack>
        <Stack spacing={2} direction={"column"} sx={{ marginTop: "25px" }}>
          {isLoading && (
            <Box sx={{ display: "flex", justifyContent: "center" }}>
              <CircularProgress />
            </Box>
          )}
          {!isLoading && (
            <Button variant="outlined" onClick={handleSendOTP}>
              Send OTP
            </Button>
          )}
          <Link to="/signin" style={{ textDecoration: "none" }}>
            <Typography sx={{ color: "#1976d2" }}>Back</Typography>
          </Link>
        </Stack>
      </Box>
    </Box>
  );
};

export default FPEmail;
