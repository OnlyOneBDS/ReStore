import { TextField } from "@mui/material";
import { useController, UseControllerProps } from "react-hook-form";

interface Props extends UseControllerProps {
  label: string;
  multiline?: boolean;
  rows?: number;
  type?: string;
}

export default function AppTextInput(props: Props) {
  const { field, fieldState } = useController({ ...props, defaultValue: "" });

  return (
    <TextField
      {...field}
      {...props}
      fullWidth
      variant="outlined"
      multiline={props.multiline}
      rows={props.rows}
      type={props.type}
      error={!!fieldState.error}
      helperText={fieldState.error?.message} />
  );
}