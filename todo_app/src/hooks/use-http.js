import axios from "axios";
import { useCallback, useState } from "react";

const useHttp = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);

  const sendRequest = useCallback(async (requestConfig, applyData) => {
    setIsLoading(true);
    setError(null);

    try {
      const response = await axios({
        method: requestConfig.method ? requestConfig.method : "GET",
        url: requestConfig.url ? requestConfig.url : null,
        headers: requestConfig.headers ? requestConfig.headers : {},
        data: requestConfig.data ? JSON.stringify(requestConfig.data) : null,
      });

      if (!response) {
        throw new Error("Request failed");
      }

      const data = await response.data;
      applyData(data);
    } catch (err) {
      setError("Something went wrong!");
    }
    setIsLoading(false);
  }, []);

  return {
    isLoading,
    error,
    sendRequest,
  };
};

export default useHttp;
