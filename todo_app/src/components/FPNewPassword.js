import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import Box from "@mui/material/Box";
import TextField from "@mui/material/TextField";
import Button from "@mui/material/Button";
import Stack from "@mui/material/Stack";
import { Link } from "react-router-dom";
import CircularProgress from "@mui/material/CircularProgress";
import { Typography } from "@mui/material";

const FPNewPassword = () => {
  const [data, setData] = useState({
    newPassword: "",
    conNewPassword: "",
  });
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState("");
  const navigate = useNavigate();

  const handleInputChange = (e) => {
    setData({ ...data, [e.target.name]: e.target.value });
  };

  const handleNewPassword = (e) => {
    e.preventDefault();
    setIsLoading(true);
    navigate("/signin");
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
            label="New Password"
            variant="outlined"
            // fullWidth
            name="newPassword"
            value={data.newPassword}
            onChange={handleInputChange}
            type="password"
          />
          <TextField
            label="Confirm New Password"
            variant="outlined"
            // fullWidth
            name="conNewPassword"
            value={data.conNewPassword}
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
            <Button variant="outlined" onClick={handleNewPassword}>
              Save
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

export default FPNewPassword;
