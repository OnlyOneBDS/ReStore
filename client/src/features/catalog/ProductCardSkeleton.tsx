import { Card, CardActions, CardContent, CardHeader, Grid, Skeleton } from "@mui/material";

export default function ProductCardSkeleton() {
  return (
    <Grid item xs component={Card}>
      <CardHeader
        avatar={<Skeleton animation="wave" height={40} width={40} variant="circular" />}
        title={<Skeleton animation="wave" height={10} width="80%" style={{ marginBottom: 6 }} />}
      />
      <Skeleton animation="wave" sx={{ height: 190 }} variant="rectangular" />
      <CardContent>
        <>
          <Skeleton animation="wave" height={10} style={{ marginBottom: 6 }} />
          <Skeleton animation="wave" height={10} width="80%" />
        </>
      </CardContent>
      <CardActions>
        <>
          <Skeleton animation="wave" height={10} width="40%" />
          <Skeleton animation="wave" height={10} width="20%" />
        </>
      </CardActions>
    </Grid>
  );
}