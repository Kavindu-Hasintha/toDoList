import React, { useEffect, useState } from "react";
import axios from "axios";
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import TodoTableStyles from "../Styles/TodoTable.module.css";
import Dialog from "@mui/material/Dialog";
import { TextField } from "@mui/material";

function TodoTable(props) {
  const [todoData, setTodoData] = useState([
    {
      id: 0,
      taskName: "",
      startDate: "",
      dueDate: "",
    },
  ]);
  const [popup, setPopup] = useState(false);
  const [addOrSave, setAddOrSave] = useState(0);
  const [taskId, setTaskId] = useState(0);
  const [taskName, setTaskName] = useState("");
  const [startDate, setStartDate] = useState(new Date());
  const [dueDate, setDueDate] = useState(new Date());

  const addBoxOpen = () => {
    setAddOrSave(0);
    emptyFields();
    setPopup(true);
  };

  const handleClose = () => {
    setPopup(false);
    emptyFields();
    window.location.reload(false);
  };

  const editBoxOpen = (todoData) => {
    setTaskId(todoData.id);
    setTaskName(todoData.taskName);
    setStartDate(todoData.startDate);
    setDueDate(todoData.dueDate);
    setAddOrSave(1);
    setPopup(true);
  };

  const emptyFields = () => {
    setTaskId(0);
    setTaskName("");
    setStartDate("");
    setDueDate("");
  };

  const handleAddOrEdit = () => {
    if (addOrSave === 0) {
      axios
        .post("https://localhost:7068/api/Todos?userId=" + props.userId, {
          taskName: taskName,
          startDate: startDate,
          dueDate: dueDate,
        })
        .then((res) => {
          toast.success(res.data);
          emptyFields();
        })
        .catch((err) => {
          toast.error("Insertion failed.");
        });
    } else {
      axios
        .put("https://localhost:7068/api/Todos", {
          id: taskId,
          taskName: taskName,
          startDate: startDate,
          dueDate: dueDate,
        })
        .then((res) => {
          toast.success(res.data);
          handleClose();
        })
        .catch((err) => {
          toast.error("Update failed.");
        });
    }
  };

  const handleDelete = (taskId) => {
    axios
      .delete("https://localhost:7068/api/Todos/" + taskId)
      .then((res) => {
        toast.success(res.data);
        window.location.reload(false);
      })
      .catch((err) => {
        toast.error("Deletion failed.");
      });
  };

  useEffect(() => {
    axios
      .get("https://localhost:7068/api/Todos/" + props.userId)
      .then((res) => {
        setTodoData(res.data);
      })
      .catch((err) => {
        toast.error("Server is not working.");
      });
  }, []);

  return (
    <div className="container bg-white w-50">
      <div className="row d-flex align-items-center py-2 border-bottom border-dark">
        <div className="col-8">
          <h2 className="text-start ps-4">toDo List</h2>
        </div>
        <div className="col-4 d-flex justify-content-end">
          <button
            type="button"
            className="btn btn-primary me-4 px-4"
            onClick={addBoxOpen}
          >
            Add
          </button>
        </div>
      </div>
      <div className="row p-3">
        <div className={TodoTableStyles.tableDiv}>
          <table className="table table-hover">
            <thead>
              <tr>
                <th scope="col">Task Name</th>
                <th scope="col">Start Date</th>
                <th scope="col">Due Date</th>
                <th scope="col">Action</th>
              </tr>
            </thead>
            <tbody>
              {todoData.map((data) => {
                return (
                  <tr key={data.id}>
                    <td>{data.taskName}</td>
                    <td>{data.startDate.substring(0, 10)}</td>
                    <td>{data.dueDate.substring(0, 10)}</td>
                    <td>
                      <button
                        className="btn btn-warning"
                        onClick={() => {
                          editBoxOpen(data);
                        }}
                      >
                        Edit
                      </button>
                      <button
                        className="btn btn-danger ms-3"
                        onClick={() => {
                          handleDelete(data.id);
                        }}
                      >
                        Delete
                      </button>
                    </td>
                  </tr>
                );
              })}
            </tbody>
          </table>
        </div>
      </div>
      <Dialog open={popup} className="task-add-dialog">
        <div className="container bg-white p-3">
          <div className="row d-flex justify-content-start mx-3">
            <h3 className="p-0">
              {addOrSave === 0 ? "Add New Task" : "Edit Task"}
            </h3>
          </div>
          <div className="row m-3">
            <TextField
              id="outlined-basic"
              label="Task Name"
              variant="outlined"
              name="taskName"
              type="text"
              placeholder="eg: Task 1"
              value={taskName}
              onChange={(e) => {
                setTaskName(e.target.value);
              }}
            />
          </div>
          <div className="row m-3">
            <TextField
              id="outlined-basic"
              label="Start Date"
              variant="outlined"
              name="startDate"
              type="text"
              placeholder="eg: 2020-01-01"
              value={startDate.toString().substring(0, 10)}
              onChange={(e) => {
                setStartDate(e.target.value);
              }}
            />
          </div>
          <div className="row m-3">
            <TextField
              id="outlined-basic"
              label="Due Date"
              variant="outlined"
              name="dueDate"
              type="text"
              placeholder="eg: 2020-01-01"
              value={dueDate.toString().substring(0, 10)}
              onChange={(e) => {
                setDueDate(e.target.value);
              }}
            />
          </div>
          <div className="row m-3">
            <button className="btn btn-success my-2" onClick={handleAddOrEdit}>
              {addOrSave === 0 ? "Add" : "Save"}
            </button>
            <button className="btn btn-secondary my-2" onClick={handleClose}>
              Cancel
            </button>
          </div>
        </div>
      </Dialog>
    </div>
  );
}

export default TodoTable;
