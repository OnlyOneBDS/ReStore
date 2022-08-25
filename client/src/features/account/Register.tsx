import LockOutlinedIcon from '@mui/icons-material/LockOutlined';
import { LoadingButton } from '@mui/lab';
import { Avatar, Box, Container, Grid, Paper, TextField, Typography } from "@mui/material";
import { useForm } from 'react-hook-form';
import { Link, useHistory } from 'react-router-dom';
import { toast } from 'react-toastify';

import agent from '../../app/api/agent';

type FormInputs = {
  userName: string;
  email: string;
  password: string;
};

export default function Register() {
  const history = useHistory();
  const { formState: { errors, isSubmitting, isValid }, handleSubmit, register, setError } = useForm<FormInputs>({ mode: "all" });

  function handleApiErrors(errors: any) {
    if (errors) {
      errors.forEach((error: string) => {
        if (error.includes("Password")) {
          setError("password", { message: error });
        }
        else if (error.includes("Email")) {
          setError("email", { message: error });
        }
        else if (error.includes("Username")) {
          setError("userName", { message: error });
        }
      });
    }
  }

  return (
    <Container component={Paper} maxWidth="sm" sx={{ display: "flex", flexDirection: "column", alignItems: "center", p: 4 }}>
      <Avatar sx={{ m: 1, bgcolor: 'secondary.main' }}>
        <LockOutlinedIcon />
      </Avatar>
      <Typography component="h1" variant="h5">
        Register
      </Typography>
      <Box component="form" onSubmit={handleSubmit((data) => agent.Account.register(data).then(() => { toast.success("Registration successful - you can now login"); history.push("/login") }).catch(error => handleApiErrors(error)))} noValidate sx={{ mt: 1 }}>
        <TextField
          margin="normal"
          fullWidth
          label="Username"
          {...register("userName", { required: "Username is required" })}
          error={!!errors.userName}
          helperText={errors?.userName?.message}
          autoFocus />
        <TextField
          margin="normal"
          fullWidth
          label="Email"
          {...register("email", { required: "Email is required", pattern: { value: /^\w+[\w-.]*@\w+((-\w+)|(\w*))\.[a-z]{2,3}$/, message: "Not a valid email address" } })}
          error={!!errors.email}
          helperText={errors?.email?.message}
          type="email" />
        <TextField
          margin="normal"
          fullWidth
          label="Password"
          {...register("password", { required: "Password is required", pattern: { value: /(?=^.{6,10}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\s).*$/, message: "Password does not meet complexity requirements" } })}
          error={!!errors.password}
          helperText={errors?.password?.message}
          type="password" />
        <LoadingButton disabled={!isValid} fullWidth loading={isSubmitting} sx={{ mt: 3, mb: 2 }} type="submit" variant="contained">
          Register
        </LoadingButton>
        <Grid container>
          <Grid item>
            <Link to="/register">
              {"Already have an account? Sign In"}
            </Link>
          </Grid>
        </Grid>
      </Box>
    </Container>
  );
};