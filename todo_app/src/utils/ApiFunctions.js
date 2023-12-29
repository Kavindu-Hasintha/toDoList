import axios from "axios";

export async function LoIn(logInData) {
  const response = await axios({
    method: "POST",
    url: "https://localhost:7068/api/Auth/signin",
    data: logInData,
  });
  return response;
}
