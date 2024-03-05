import React, { useState } from "react";
import Box from "@mui/material/Box";
import TextField from "@mui/material/TextField";
import Button from "@mui/material/Button";
import Stack from "@mui/material/Stack";
import { Link } from "react-router-dom";
import CircularProgress from "@mui/material/CircularProgress";
import { useNavigate } from "react-router-dom";
import { Typography } from "@mui/material";

const FPOTP = () => {
  const [otp, setOtp] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState("");
  const navigate = useNavigate();

  const handleInputChange = (e) => {
    setOtp(e.target.value);
  };

  const handleEnteredOTP = (e) => {
    e.preventDefault();
    setIsLoading(true);
    navigate("/fp-new-password");
    setIsLoading(false);
  };

  const handleResendOtp = () => {};

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
            label="Enter Your OTP"
            variant="outlined"
            // fullWidth
            name="otp"
            value={otp}
            onChange={handleInputChange}
            type="email"
          />
        </Stack>
        <Stack
          spacing={2}
          direction={"column"}
          sx={{ marginTop: "25px", display: "flex", justifyContent: "center" }}
        >
          {isLoading && (
            <Box sx={{ display: "flex", justifyContent: "center" }}>
              <CircularProgress />
            </Box>
          )}
          {!isLoading && (
            <Button variant="outlined" onClick={handleEnteredOTP}>
              Confirm
            </Button>
          )}
          <Button variant="text" onClick={handleResendOtp}>
            Resend
          </Button>
          <Link to="/forget-password" style={{ textDecoration: "none" }}>
            <Typography sx={{ color: "#1976d2" }}>Back</Typography>
          </Link>
        </Stack>
      </Box>
    </Box>
  );
};

export default FPOTP;
