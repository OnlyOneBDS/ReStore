import LockOutlinedIcon from '@mui/icons-material/LockOutlined';
import { LoadingButton } from '@mui/lab';
import { Avatar, Box, Container, Grid, Paper, TextField, Typography } from "@mui/material";
import { useForm } from 'react-hook-form';
import { Link, useHistory, useLocation } from 'react-router-dom';

import { useAppDispatch } from '../../app/store/configureStore';
import { signInUser } from './accountSlice';

type FormInputs = {
  userName: string;
  password: string;
};

export default function Login() {
  const history = useHistory();
  const location = useLocation<any>();
  const dispatch = useAppDispatch();
  const { formState: { errors, isSubmitting, isValid }, handleSubmit, register } = useForm<FormInputs>({ mode: "all" });

  async function submitForm(data: FormInputs) {
    try {
      await dispatch(signInUser(data));
      history.push(location.state?.from?.pathname || "/catalog");
    }
    catch (error) {
      console.log(error);
    }
  }

  return (
    <Container component={Paper} maxWidth="sm" sx={{ display: "flex", flexDirection: "column", alignItems: "center", p: 4 }}>
      <Avatar sx={{ m: 1, bgcolor: 'secondary.main' }}>
        <LockOutlinedIcon />
      </Avatar>
      <Typography component="h1" variant="h5">
        Sign in
      </Typography>
      <Box component="form" onSubmit={handleSubmit(submitForm)} noValidate sx={{ mt: 1 }}>
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
          label="Password"
          {...register("password", { required: "Password is required" })}
          error={!!errors.password}
          helperText={errors?.password?.message}
          type="password" />
        <LoadingButton disabled={!isValid} fullWidth loading={isSubmitting} sx={{ mt: 3, mb: 2 }} type="submit" variant="contained">
          Sign In
        </LoadingButton>
        <Grid container>
          <Grid item>
            <Link to="/register">
              {"Don't have an account? Sign Up"}
            </Link>
          </Grid>
        </Grid>
      </Box>
    </Container>
  );
};