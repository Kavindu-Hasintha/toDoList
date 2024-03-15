import axios from "axios";

export const API_URL = "https://localhost:7068";

const getToken = () => {
  return localStorage.getItem("token");
};

const getRefreshToken = () => {
  return localStorage.getItem("refreshToken");
};

export async function SignIn(signInData) {
  //try {
  const response = await axios({
    method: "POST",
    url: API_URL + "/api/Auth/signin",
    data: signInData,
  });

  return response;
  // } catch (error) {
  //   console.log("a - " + error);
  //   throw error;
  // }
}
