import axios from "axios";

export const API_URL = "https://localhost:7068";

const getToken = () => {
  return localStorage.getItem("token");
};

const getRefreshToken = () => {
  return localStorage.getItem("refreshToken");
};

export async function SignIn(signInData) {
  const response = await axios({
    method: "POST",
    url: API_URL + "/api/Auth/signin",
    data: signInData,
  });
  return response;
}

export async function SignUp(signUpData) {
  const response = await axios({
    method: "POST",
    url: API_URL + "/api/Auth/signup",
    data: signUpData,
  });
  return response;
}
